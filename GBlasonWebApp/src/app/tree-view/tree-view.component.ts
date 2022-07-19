import { HttpClient, HttpEvent, HttpRequest, HttpResponse } from '@angular/common/http';
import { AfterViewInit, Component, Renderer2, ElementRef, Input, OnInit, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { TreeViewNode } from '../ebnf/ebnf.component';
import { Point, TreeViewNodeSVG, TreeViewUINode } from './TreeViewUINode';

@Component({
  selector: 'app-tree-view',
  templateUrl: './tree-view.component.html',
  styleUrls: ['./tree-view.component.scss']
})

export class TreeViewComponent implements OnInit, AfterViewInit, OnChanges {

  constructor(private http: HttpClient, private renderer: Renderer2) { }

  /**
   * View child of tree view component
   * canvas is the parent div container of the whole tree used to add the root(s) and to fill the display area
   * svg is the parent svg container of the whole tree, used to move the tree as one element
   */
  @ViewChild('canvas')
  canvasDom: ElementRef | null = null;
  @ViewChild('svg')
  svgDom: ElementRef | null = null;

  /**
   * Input  of tree view component
   * treeHead is the root TreeViewNode of the tree to be displayed, (pure object not transformed for displaying purpose yet)
   */
  @Input()
  treeHead: TreeViewNode | null = null;

  /**
   * Root node ui of tree view component representing the actual displayed object and its properties in the UI tree
   */
  private rootNodeUi: TreeViewUINode | null = null;

  private _renderingReady: boolean = false;
  private _dataReady: boolean = false;

  private _xCounter: number = 0;

  private _startDragPoint: Point = { x: 0, y: 0 };
  private _offset: Point = { x: 0, y: 0 };
  private _dragStarted: boolean = false;

  private _canvasMatrix = [1, 0, 0, 1, 0, 0];

  //#region event handlers

  ngOnInit(): void {

  }

  ngOnChanges(changes: SimpleChanges) {
    //the data arrived in we need to reset the whole content
    this._dataReady = this.treeHead != null;
    if (this.treeHead != null) { this.updateUi(null, [this.treeHead]); }
  }

  ngAfterViewInit(): void {
    //first rendering of pure svg
    //we need to unblock or start the promise to initialize the content
    this._renderingReady = true
    if (this.treeHead != null) { this.updateUi(null, [this.treeHead]); }
  }

  onStartDrag(event: PointerEvent) {
    if (event == null) {
      return;
    }
    this._startDragPoint = { x: event.clientX, y: event.clientY };
    this._dragStarted = true;
  }

  onDrag(event: any) {
    if (event == null || !this._dragStarted) {
      return;
    }
    event.preventDefault();

    //we calculate the difference of the current position vs the last position and pan by that much
    this.pan(event.clientX - this._startDragPoint.x, event.clientY - this._startDragPoint.y);
    //refresh the start position to the last event (this one)
    this._startDragPoint = { x: event.clientX, y: event.clientY };
  }

  onStopDrag(event: any) {
    //the new general position of the frame is the final offset
    if (!this._dragStarted) {
      return;
    }
    this._dragStarted = false;
  }

  onWheel(event: any) {
    if (event == null) {
      return;
    }
    //we try to zoom out or in depending on the event
    var delta = event.wheelDelta;
    this.zoomFactor(delta);
  }

  /**
   * Handle the click on the center button, by placing the head (root) node at the y axis top and x axis center
   * @returns nothing
   */
  onCenterClick() {
    if (this.rootNodeUi == null || this.rootNodeUi.svgNode == null) {
      return;
    }
    this.centerToNode(this.rootNodeUi);
  }

  onExpandAllClick() {
    //in this version we only expand the nodes that have already gotten their children

    if (this.rootNodeUi == null || this.rootNodeUi.svgNode == null) {
      return;
    }
    this.expandFrom(this.rootNodeUi);
  }

  /**
   * Handle the click on the expand button in the node card.
   * @param event the event sent by the UI
   */
  onNodeExpandClick(node: TreeViewUINode) {

    if (node == null) {
      return;
    }
    //(if this is a tree leaf, then we do nothing)
    if (node.treeNode.RealElement?.IsLeaf ?? false) {
      return;
    }

    console.log(`tree-view-component.onNodeExpandClick(node: ${node.treeNode.RealElement?.Name})`);

    //there are 2 options, this is either an expand or a collapse
    //if there are no children and this is not a lead, then we likely need to request the children
    if (node.children.length == 0 && !node.treeNode.RealElement?.IsLeaf) {
      this.getMoreNode(node);
    }
    else {
      //just a play on the button states and the visibility of the nodes
      node.expand(!node.isExpanded);
    }
  }

  onCollapseAllClick() {
    if (this.rootNodeUi == null || this.rootNodeUi.svgNode == null) {
      return;
    }
    this.expandFrom(this.rootNodeUi, false);
  }

  onZoomToFitClick() {
    if (this.rootNodeUi == null || this.rootNodeUi.svgNode == null) {
      return;
    }
    //finding out what is the current size of the canvas
    var canvasInfo = this.canvasDom?.nativeElement.getBoundingClientRect();
    //finding out what is the current container size
    var svgInfo = this.svgDom?.nativeElement.getBoundingClientRect();

    //I just need to set the svg viewbox to the height and width of the canvasdom element
    //only when the size of the canvasDom in absolute unit is bigger (either height or width) than the svg
    //so the svg size is the minimum
    //and with a predefined maximum width based on how small it gets from pratical experience

    var ratioWidth = svgInfo.width / canvasInfo.width;
    var ratioHeight = svgInfo.height / canvasInfo.height;
  }

  //#endregion

  centerToNode(node: TreeViewUINode) {

    if (node.svgNode == null) {
      return;
    }

    //we just need the distance between the current location of the node, and the actual dead middle
    var currentNodeLocation = node.svgNode.domObject.getCTM();
    var currentNodeSize = node.svgNode.domObject.getBoundingClientRect();

    var currentSvgSize = this.svgDom?.nativeElement.getBoundingClientRect();

    //the ideal (centered) position of the head should be

    var targetPosition: Point = {
      x: currentSvgSize.width / 2 - currentNodeSize.width / 2,
      y: node.svgNode._node_margin //we could add a small padding for the sake of UI comfort rather than 0 ...
    };

    var log = `tree-view-component.centerToNode(node: ${node.treeNode.RealElement?.Name})`;
    log += `\n\t> current node location: (${currentNodeLocation.e},${currentNodeLocation.f})`;
    log += `\n\t> current svg size: (${currentSvgSize.width},${currentSvgSize.height})`;
    log += `\n\t> current target: (${targetPosition.x},${targetPosition.y})`;

    console.log(log);
    //we pass the difference in location to the pan to move the node within the expected position
    this.pan(targetPosition.x - currentNodeLocation.e, targetPosition.y - currentNodeLocation.f);
  }

  getMoreNode(head: TreeViewUINode) {
    var httpRequest = new HttpRequest("GET", "api/ebnf/tree?head=" + head.treeNode.ElementId, { responseType: "text" });
    var request = this.http.request<string>(httpRequest);

    //ideally we should add an animated SVG to replace the button while the request is in progress (for later)
    request.subscribe({
      next: (data: HttpEvent<string>) => {
        var result = data as HttpResponse<string>;
        if (result !== null && result.body !== null && result.body !== undefined) {
          //we received new raw data, we need to parse and add them to the tree
          var replacement = JSON.parse(result.body);
          //we should receive the tree with the current head as the subtree head
          if (replacement.ElementId === head.treeNode.ElementId) {
            //we are good, and the object to update is not the parent we received but its children
            this.updateUi(head, replacement.Children);
          } else {
            throw new Error(`the received head ${replacement.ElementId} is different than the expected one ${head.treeNode.ElementId}`);
          }
        }
      },
      error: (err) => { },
      complete: () => {
        head.expand(true);
      }
    });
  }

  /**
   * Pans the SVG canvas to the direction given by the dx and dy
   * @param dx the distance to move on the x axis
   * @param dy the distance to move on the y axis
   */
  private pan(dx: number, dy: number) {
    this._canvasMatrix[4] += dx;
    this._canvasMatrix[5] += dy;

    this._offset.x += dx;
    this._offset.y += dy;

    var newMatrix = `matrix(${this._canvasMatrix.join(' ')})`;
    this.renderer.setAttribute(this.canvasDom?.nativeElement, "transform", newMatrix);
  }

  /**
   * Zooms factor
   * @param factor
   */
  private zoomFactor(factor: number) {
    console.log(`tree-view-component.zoomFactor(factor: ${factor})`);
  }



  /**
   * Calculates the x axis positions for all the nodes in the tree
   */
  calculateXPositions(): void {
    //we need to go through the tree, for all leaves we add the x value in the counter and increase it by 1
    //at the end of this the direct parent get a value which is the average of the children
    if (this.treeHead == null) {
      return;
    }
    this._xCounter = 0;
    this.calculateXPositionsStep(this.rootNodeUi);
  }

  private expandFrom(node: TreeViewUINode, expand: boolean = true) {
    if (node == null || node.children.length == 0) {
      return;
    }
    node.expand(expand);
    for (var i = 0; i < node.children.length; i++) {
      this.expandFrom(node.children[i], expand);
    }
  }

  /**
   * Calculates the index of all the nodes on the x axis, the leaves takes all one x natural value each, their parents are the average of their children's positions
   * This is used later to calculate the position of each node and avoid them touching or overlapping each other
   * @param node the node from which to calculate (the head of the tree)
   * @returns nothing, when the node xposition value has been set
   */
  calculateXPositionsStep(node: TreeViewUINode | null) {
    if (node == null) {
      return;
    }
    if ((node.children?.length ?? 1) == 0) {
      node.isLeaf = true;
      node.xPosition = this._xCounter++;
    }
    else {
      var parentX = 0;
      for (var i = 0; i < node.children.length; i++) {
        var child = node.children[i];
        this.calculateXPositionsStep(child);
        parentX += child.xPosition;
      }
      node.xPosition = parentX / node.children.length;
    }
  }

  /**
   * Refresh the whole UI by calculating the new UI tree with all nodes, recalculating their position, adding the new nodes and transforming the actual SVG nodes positions
   * @param parent the parent from which the update needs to happen (the rendering is always from the first root)
   * @param children the new nodes that needs to be added (rendered) into the tree
   */
  updateUi(parent: TreeViewUINode | null, children: TreeViewNode[]): void {
    var newHeads = this.buildUiTree(parent, children);
    this.calculateXPositions();
    if (this.rootNodeUi != null) {
      console.log(`tree-view-component.updateUi(parent: ${parent?.treeNode.RealElement?.Name}, children: [${children.length}])`);
      this.renderTree(null, [this.rootNodeUi]);
      var canvasInfo = this.canvasDom?.nativeElement.getBoundingClientRect();
    }
  }

  /**
   * Build the tree of logical nodes by attaching the node logical subtree to the parent node (logical ui objects that are ready to render)
   * @param parent the node that should receive the new subtree
   * @param node the head or heads of the new subtree
   * @returns the TreeViewUINode generated by the tree building call or null if the creation could not happen
   */
  buildUiTree(parent: TreeViewUINode | null, node: TreeViewNode[] | null): TreeViewUINode[] {
    if (node === null || node.length === 0 || this._renderingReady === false || this._dataReady === false) {
      return [];
    }
    var nodeUi: TreeViewUINode[] = [];
    if (parent == null) {
      //special unique case !
      //if the current parent node has no parent, it is a root, and we set it at the center
      this.rootNodeUi = new TreeViewUINode(this.renderer, node[0]);
      nodeUi.push(this.rootNodeUi);
      //and execute the same for the child of the current node
      if (node[0].Children !== null && (node[0].Children?.length ?? 0) > 0) {
        this.buildUiTree(this.rootNodeUi, node[0].Children);
      }
    } else {
      //if it does however, we need to "attach" all of the nodes as the "descendent" from a tree view representation perspective of the current parent
      for (var i = 0; i < node.length; i++) {
        var newNodeUi = new TreeViewUINode(this.renderer, node[i], parent);
        parent.children.push(newNodeUi);
        nodeUi.push(newNodeUi);
      }
    }
    return nodeUi;
  }

  /**
   * Renders the tree by creating new nodes and transforming all the nodes to their new position
   * @param parent
   * @param node
   * @returns nothing
   */
  renderTree(parent: TreeViewUINode | null, node: TreeViewUINode[]): void {
    if (node === null || node.length === 0 || this._renderingReady === false || this._dataReady === false) {
      return;
    }
    var log = `tree-view-component.renderTree(parent: ${parent?.treeNode.RealElement?.Name}, node[]:{${node[0].treeNode.RealElement?.Name}`;
    for (var i = 1; i < node.length; i++) {
      log += ", " + node[i].treeNode.RealElement?.Name;
    }
    log += "})";
    console.log(log);

    var renderedNode: TreeViewNodeSVG | null = null;
    //we attach all the nodes in the tree, trying to render the new ones and update the existing ones
    for (var i = 0; i < node.length; i++) {
      renderedNode = node[i].renderNode(parent);
      if (renderedNode != null) {
        console.log(`tree-view-component.renderTree.addEventListener(node: ${node[i].treeNode.RealElement?.Name})`);
        renderedNode.addEventListener("expandButtonClick", this.onNodeExpandClick.bind(this, node[i]));
        this.renderer.appendChild(this.canvasDom?.nativeElement, renderedNode.domObject);
      }
    }
    //and execute the same for the child of the current node and after the children are created we do the parent links
    for (var i = 0; i < node.length; i++) {
      if (node[i].children !== null && node[i].children.length > 0) {
        console.log(`tree-view-component.renderTree -SubCall - (parent: ${node[i]?.treeNode.RealElement?.Name}, children: [${node[i].children.length}])`);
        this.renderTree(node[i], node[i].children);
      }
    }
    //we need to place all the children before we calculate the positions of the links
    for (var i = 0; i < node.length; i++) {
      var linkResult = node[i].renderLinks();
      if (linkResult != null) {
        this.renderer.insertBefore(this.canvasDom?.nativeElement, linkResult, node[i].svgNode?.domObject);
      }
    }
  }
}

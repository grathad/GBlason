import { TreeViewNode } from '../ebnf/ebnf.component';
import { Renderer2 } from "@angular/core";

export interface Point {
  x: number;
  y: number;
}

interface IconInfo {
  path: string;
  active: boolean;
  tooltip: string;
}

/**
 * Tree view uinode
 */

export class TreeViewUINode {

  readonly treeNode: TreeViewNode;
  isVisible: boolean = true;
  svgNode: TreeViewNodeSVG | null = null;
  transformation: Point = { x: 0, y: 0 };
  position: Point | null = null;
  depth: number = 0;
  isLeaf: boolean = true;
  xPosition: number = 0;
  children: TreeViewUINode[] = [];
  isExpanded: boolean = false;
  expansionIcon: any | null = null;

  /**
   *
   */
  constructor(renderer: Renderer2, treeNode: TreeViewNode, parent: TreeViewUINode | null = null) {
    this.treeNode = treeNode;
    this.svgNode = new TreeViewNodeSVG(renderer);
    if (treeNode != null && (treeNode.Children?.length ?? 0) > 0) {
      this.isLeaf = false;
    }
    if (parent != null) {
      this.depth = parent.depth + 1;
    }
  }

  expand(expansion: boolean) {
    this.isExpanded = expansion;
    this.svgNode?.expand(expansion);
    //we make the direct child visible as well
    for (var i = 0; i < this.children.length; i++) {
      this.children[i].setVisibility(expansion);
    }
  }

  private setVisibility(visible: boolean) {
    this.isVisible = visible;
    this.svgNode?.setVisibility(visible);
    //we only apply the visibility to the children based on the current expansion status (or the expansion status of the parent, meaning that the visibility intent AND the expansion status of the parent matter)
    for (var i = 0; i < this.children.length; i++) {
      this.children[i].setVisibility(this.isExpanded && this.isVisible);
    }
  }

  placeNode() {
    this.transformation = this.svgNode?.placeNode(this) ?? { x: 0, y: 0 };
    console.log(`TreeViewUINode.placeNode for ${this.treeNode.RealElement?.Name} returned: x index: (${this.xPosition}), depth: (${this.depth}) at coordinates: (${this.transformation.x},${this.transformation.y})`);
  }

  renderNode(parent: TreeViewUINode | null): TreeViewNodeSVG | null {
    console.log(`TreeViewUINode.renderNode(this: ${this.treeNode.RealElement?.Name} ,parent: ${parent?.treeNode.RealElement?.Name})`);
    var newNode = this.svgNode?.render(this, parent);
    this.placeNode();
    return newNode ? this.svgNode : null;
  }

  renderLinks(): TreeViewNodeSVG | null {
    console.log(`TreeViewUINode.renderLinks(parent: ${this.treeNode.RealElement?.Name})`);
    this.svgNode?.createLinks(this);
    return this.svgNode?.linksObject;
  }
}


export class TreeViewNodeSVG extends EventTarget {

  //24*24 icons and svgs
  private _finger_print_icon: string = "M12.025 2.025q1.6 0 3.125.387 1.525.388 2.95 1.113.225.1.263.287.037.188-.038.363t-.25.275q-.175.1-.425-.025-1.325-.675-2.738-1.038-1.412-.362-2.887-.362-1.45 0-2.85.337-1.4.338-2.675 1.063-.2.125-.387.062-.188-.062-.313-.262-.075-.2-.037-.363Q5.8 3.7 6 3.575q1.4-.75 2.925-1.15 1.525-.4 3.1-.4Zm0 2.45q2.65 0 5 1.137Q19.375 6.75 20.95 8.9q.175.225.112.4-.062.175-.212.3t-.35.112q-.2-.012-.35-.212-1.375-1.95-3.537-2.988-2.163-1.037-4.588-1.037-2.425 0-4.55 1.037Q5.35 7.55 3.95 9.5q-.15.225-.35.25-.2.025-.35-.1-.175-.125-.225-.313-.05-.187.125-.387 1.525-2.125 3.875-3.3 2.35-1.175 5-1.175Zm0 4.775q2.325 0 4 1.562Q17.7 12.375 17.7 14.65q0 .225-.138.362-.137.138-.362.138-.2 0-.35-.138-.15-.137-.15-.362 0-1.875-1.388-3.138-1.387-1.262-3.287-1.262t-3.263 1.262Q7.4 12.775 7.4 14.65q0 2.025.7 3.437.7 1.413 2.05 2.838.15.15.15.35 0 .2-.15.35-.15.15-.35.15-.2 0-.35-.15-1.475-1.55-2.262-3.163Q6.4 16.85 6.4 14.65q0-2.275 1.65-3.838Q9.7 9.25 12.025 9.25ZM12 14.15q.225 0 .363.15.137.15.137.35 0 1.875 1.35 3.075 1.35 1.2 3.15 1.2.15 0 .425-.025t.575-.075q.225-.05.388.062.162.113.212.338.05.2-.075.35-.125.15-.325.2-.45.125-.787.138-.338.012-.413.012-2.225 0-3.863-1.5-1.637-1.5-1.637-3.775 0-.2.137-.35.138-.15.363-.15Zm.025-2.45q1.275 0 2.175.85.9.85.9 2.1 0 .825.625 1.388.625.562 1.475.562.85 0 1.45-.562.6-.563.6-1.388 0-2.9-2.125-4.875T12.05 7.8q-2.95 0-5.075 1.975t-2.125 4.85q0 .6.113 1.5.112.9.537 2.1.075.225-.012.4-.088.175-.288.25-.2.075-.388-.013-.187-.087-.262-.287-.375-.975-.537-1.938-.163-.962-.163-1.987 0-3.325 2.413-5.575 2.412-2.25 5.762-2.25 3.375 0 5.8 2.25t2.425 5.575q0 1.25-.887 2.088-.888.837-2.163.837t-2.187-.837Q14.1 15.9 14.1 14.65q0-.825-.612-1.388-.613-.562-1.463-.562-.85 0-1.463.562-.612.563-.612 1.388 0 2.425 1.438 4.05 1.437 1.625 3.712 2.275.225.1.3.262.075.163.025.363-.05.175-.2.3-.15.125-.375.075-2.6-.675-4.25-2.6T8.95 14.65q0-1.25.9-2.1.9-.85 2.175-.85Z";
  private _node_card_shape: string = "M0,0 h180 a4,4 0 0 1 4,4 v132 a4,4 0 0 1 -4,4 h-180 a4,4 0 0 1 -4,-4 v-132 a4,4 0 0 1 4,-4 z";
  private _branch_icon: string = "M11 22v-5q0-1.4-.425-2.075-.425-.675-1.125-1.325l1.425-1.425q.3.275.575.587.275.313.55.663.35-.475.713-.838.362-.362.737-.712.95-.875 1.725-2.025.775-1.15.825-4.025L14.425 7.4 13 6l4-4 4 4-1.4 1.4L18 5.825q-.05 3.575-1.1 5.087-1.05 1.513-2.1 2.463-.8.725-1.3 1.412-.5.688-.5 2.213v5ZM6.2 8.175q-.1-.5-.138-1.1-.037-.6-.062-1.25L4.4 7.4 3 6l4-4 4 4-1.425 1.4L8 5.85q0 .525.05.988.05.462.1.862Zm2.15 4.4q-.5-.525-.962-1.225-.463-.7-.813-1.725L8.5 9.15q.25.675.575 1.15.325.475.7.85Z";
  private _repeat_icon: string = "M11 20.95q-3.025-.375-5.012-2.638Q4 16.05 4 13q0-1.65.65-3.163Q5.3 8.325 6.5 7.2l1.425 1.425q-.95.85-1.437 1.975Q6 11.725 6 13q0 2.2 1.4 3.887 1.4 1.688 3.6 2.063Zm2 0v-2q2.175-.4 3.587-2.075Q18 15.2 18 13q0-2.5-1.75-4.25T12 7h-.075l1.1 1.1-1.4 1.4-3.5-3.5 3.5-3.5 1.4 1.4-1.1 1.1H12q3.35 0 5.675 2.325Q20 9.65 20 13q0 3.025-1.987 5.288Q16.025 20.55 13 20.95Z";
  private _optional_icon: string = "M10.6 16q0-2.025.363-2.913.362-.887 1.537-1.937 1.025-.9 1.562-1.563.538-.662.538-1.512 0-1.025-.687-1.7Q13.225 5.7 12 5.7q-1.275 0-1.938.775-.662.775-.937 1.575L6.55 6.95q.525-1.6 1.925-2.775Q9.875 3 12 3q2.625 0 4.038 1.463 1.412 1.462 1.412 3.512 0 1.25-.537 2.138-.538.887-1.688 2.012Q14 13.3 13.738 13.912q-.263.613-.263 2.088Zm1.4 6q-.825 0-1.412-.587Q10 20.825 10 20q0-.825.588-1.413Q11.175 18 12 18t1.413.587Q14 19.175 14 20q0 .825-.587 1.413Q12.825 22 12 22Z";
  private _group_icon: string = "M8 16q.825 0 1.413-.588Q10 14.825 10 14t-.587-1.413Q8.825 12 8 12q-.825 0-1.412.587Q6 13.175 6 14q0 .825.588 1.412Q7.175 16 8 16Zm8 0q.825 0 1.413-.588Q18 14.825 18 14t-.587-1.413Q16.825 12 16 12q-.825 0-1.412.587Q14 13.175 14 14q0 .825.588 1.412Q15.175 16 16 16Zm-4-6q.825 0 1.413-.588Q14 8.825 14 8t-.587-1.412Q12.825 6 12 6q-.825 0-1.412.588Q10 7.175 10 8t.588 1.412Q11.175 10 12 10Zm0 12q-2.075 0-3.9-.788-1.825-.787-3.175-2.137-1.35-1.35-2.137-3.175Q2 14.075 2 12t.788-3.9q.787-1.825 2.137-3.175 1.35-1.35 3.175-2.138Q9.925 2 12 2t3.9.787q1.825.788 3.175 2.138 1.35 1.35 2.137 3.175Q22 9.925 22 12t-.788 3.9q-.787 1.825-2.137 3.175-1.35 1.35-3.175 2.137Q14.075 22 12 22Zm0-2q3.35 0 5.675-2.325Q20 15.35 20 12q0-3.35-2.325-5.675Q15.35 4 12 4 8.65 4 6.325 6.325 4 8.65 4 12q0 3.35 2.325 5.675Q8.65 20 12 20Zm0-8Z";
  private _leaf_icon: string = "M5.4 19.6Q4.275 18.475 3.638 17 3 15.525 3 13.95q0-1.575.6-3.113Q4.2 9.3 5.55 7.95q.875-.875 2.163-1.5Q9 5.825 10.762 5.462q1.763-.362 4.026-.437 2.262-.075 5.062.175.2 2.65.125 4.875-.075 2.225-.413 4.012-.337 1.788-.949 3.125Q18 18.55 17.1 19.45q-1.325 1.325-2.812 1.937Q12.8 22 11.25 22q-1.625 0-3.175-.637-1.55-.638-2.675-1.763Zm2.8-.4q.725.425 1.488.612.762.188 1.562.188 1.15 0 2.275-.462 1.125-.463 2.15-1.488.45-.45.912-1.262.463-.813.801-2.126.337-1.312.512-3.174.175-1.863.05-4.438-1.225-.05-2.762-.038-1.538.013-3.063.238-1.525.225-2.9.725T6.975 9.35q-1.125 1.125-1.55 2.225Q5 12.675 5 13.7q0 1.475.562 2.587.563 1.113.988 1.563 1.05-2 2.775-3.838Q11.05 12.175 13.35 11q-1.8 1.575-3.137 3.562Q8.875 16.55 8.2 19.2Zm0 0Zm0 0Z";
  private _expand_less_icon: string = "M5 13v-2h14v2Z";
  private _expand_more_icon: string = "M11 19v-6H5v-2h6V5h2v6h6v2h-6v6Z";

  //constants of the node UI
  private _node_width = 180;
  private _node_height = 132;
  private _node_margin = 28;
  private _node_roundAngle = 4;
  private _node_line_thickness_center_delta = 1; //half of the thickness rounded down

  domObject: any | null = null;
  expansionIcon: any | null = null;
  linksObject: any | null = null;
  linkHorizontalLine: any | null = null;
  linkChildrenVertices: any[] = [];

  private _expandClick: Event = new Event('expandButtonClick');

  /**
   *
   */
  constructor(private renderer: Renderer2) {
    super();
  }

  expand(expansion: boolean) {
    if (this.domObject == null) {
      return;
    }
    if (expansion) {
      this.renderer.setAttribute(this.expansionIcon, "d", this._expand_less_icon);
      if (this.linksObject != null) {
        this.renderer.setAttribute(this.linksObject, "class", "node-link");
      }
    }
    else {
      this.renderer.setAttribute(this.expansionIcon, "d", this._expand_more_icon);
      if (this.linksObject != null) {
        this.renderer.setAttribute(this.linksObject, "class", "node-link hidden");
      }
    }
  }

  setVisibility(visible: boolean) {
    if (this.domObject != null) {
      if (visible) { this.renderer.setAttribute(this.domObject, "class", "node"); }
      else { this.renderer.setAttribute(this.domObject, "class", "node hidden"); }
    }
    if (this.linksObject != null) {
      if (visible) {
        this.renderer.setAttribute(this.linksObject, "class", "node-link");
      } else {
        this.renderer.setAttribute(this.linksObject, "class", "node-link hidden");
      }
    }
    return;
  }

  placeNode(currentNodeInfo: TreeViewUINode): Point {
    if (this.domObject == null) {
      return { x: 0, y: 0 };
    }
    var actualWidth = this._node_width + this._node_roundAngle * 2;
    var actualHeight = this._node_height + this._node_roundAngle * 2;
    var xPos = currentNodeInfo.xPosition * (actualWidth + this._node_margin) + this._node_margin;
    var yPos = currentNodeInfo.depth * (actualHeight + this._node_margin) + this._node_margin;
    this.renderer.setAttribute(this.domObject, "transform", `translate(${xPos},${yPos})`);
    if (this.linksObject != null) {
      //the problem with the links is that the place only is not enough, they might need to expand or shrink based on visibility of the children nodes
      this.placeLinks(currentNodeInfo);
      this.renderer.setAttribute(this.linksObject, "transform", `translate(${xPos},${yPos})`);
    }
    return { x: xPos, y: yPos };
  }

  render(currentNodeInfo: TreeViewUINode, parentNode: TreeViewUINode | null): boolean {

    var newObject = false;
    if (this.domObject == null) {
      this.createNewNode(currentNodeInfo);
      newObject = true;
      if (parentNode !== null && (!parentNode.isExpanded || !parentNode.isVisible)) {
        this.renderer.setAttribute(this.domObject, "class", "node hidden");
      }
      console.log(`TreeViewNodeSVG.render(node: ${currentNodeInfo.treeNode.RealElement?.Name}, parentNode: ${parentNode?.treeNode.RealElement?.Name}) at ${currentNodeInfo.transformation.x},${currentNodeInfo.transformation.y}`);
    }
    return newObject;
  }

  /**
   * Renders links is called when the children of a node are being rendered and a link between the parents and the children is required
   * the link is built in the UI as a different group as the node, but belong to the same UI object node as the parent
   * @childrenCenterX the list of the xPosition of the children to link at the time of the creation
   */
  createLinks(currentNodeInfo: TreeViewUINode) {
    if (currentNodeInfo == null || currentNodeInfo.children.length == 0) {
      return;
    }
    if (this.linksObject == null) {
      this.linksObject = this.renderer.createElement("g", "svg");

      if (currentNodeInfo.isVisible && currentNodeInfo.isExpanded) {
        this.renderer.setAttribute(this.linksObject, "class", "node-link");
      } else {
        this.renderer.setAttribute(this.linksObject, "class", "node-link hidden");
      }

      var parentBranch = this.renderer.createElement("line", "svg");
      this.linkHorizontalLine = this.renderer.createElement("line", "svg");

      var currentCenter = this._node_width / 2;
      var currentHeight = this._node_height + this._node_roundAngle * 2;
      var widthYAxis = (currentHeight + this._node_margin / 2);

      //the first one is the line going out of the node never moving outside of the parent, so no need to recalculate
      this.renderer.setAttribute(parentBranch, "x1", currentCenter.toString());
      this.renderer.setAttribute(parentBranch, "y1", currentHeight.toString());
      this.renderer.setAttribute(parentBranch, "x2", currentCenter.toString());
      this.renderer.setAttribute(parentBranch, "y2", (widthYAxis - this._node_line_thickness_center_delta).toString());

      this.renderer.appendChild(this.linksObject, parentBranch);
      this.renderer.appendChild(this.linksObject, this.linkHorizontalLine);

      for (var i = 0; i < currentNodeInfo.children.length; i++) {
        if (currentNodeInfo.children.length > 1 && i == 0 || i == currentNodeInfo.children.length - 1) {
          var newKid = this.renderer.createElement("path", "svg");
        }
        else {
          var newKid = this.renderer.createElement("line", "svg");
        }
        this.linkChildrenVertices.push(newKid);
        this.renderer.appendChild(this.linksObject, newKid);
      }
    }
    this.placeLinks(currentNodeInfo);
  }

  private placeLinks(currentNodeInfo: TreeViewUINode) {

    this.renderer.setAttribute(this.linksObject, "transform", `translate(${currentNodeInfo.transformation.x},${currentNodeInfo.transformation.y})`);

    var coordinates: number[] = [];
    for (var i = 0; i < currentNodeInfo.children.length; i++) {
      coordinates[i] = currentNodeInfo.children[i].transformation.x;
    }
    //current center of the current node
    var currentHeight = this._node_height + this._node_roundAngle * 2;

    var leftChildX = coordinates[0] - currentNodeInfo.transformation.x + this._node_width / 2;
    var rightChildX = (coordinates[coordinates.length - 1] - coordinates[0]) + leftChildX;
    var roundedCorners = false;
    if (currentNodeInfo.children.length > 1) {
      leftChildX += 2 * this._node_roundAngle;
      rightChildX -= this._node_roundAngle;
      roundedCorners = true;
    }
    var widthYAxis = (currentHeight + this._node_margin / 2);

    //the line can extend or shrink, need some savings
    this.renderer.setAttribute(this.linkHorizontalLine, "x1", leftChildX.toString());
    this.renderer.setAttribute(this.linkHorizontalLine, "y1", widthYAxis.toString());
    this.renderer.setAttribute(this.linkHorizontalLine, "x2", rightChildX.toString());
    this.renderer.setAttribute(this.linkHorizontalLine, "y2", widthYAxis.toString());
    //this.renderer.setAttribute(this.linkHorizontalLine, "class", "node-link");
    console.log(`TreeViewNodeSVG.placeLinks(${currentNodeInfo.treeNode.RealElement?.Name}) width total is ${rightChildX - leftChildX} left is ${leftChildX} top is ${widthYAxis}`)

    //one line per child, they also need to move later on, so we need to save them as well
    for (var i = 0; i < coordinates.length; i++) {
      var kidX = coordinates[i] - currentNodeInfo.transformation.x + this._node_width / 2;
      var newKid = this.linkChildrenVertices[i];
      this.linkChildrenVertices.push(newKid);

      var childYTop = (widthYAxis + this._node_line_thickness_center_delta);
      var childYBottom = childYTop + this._node_margin / 2;

      //this.renderer.setAttribute(newKid, "class", "node-link");


      if (roundedCorners && i == 0) {
        childYTop--;
        //this is the left corner
        this.renderer.setAttribute(newKid, "transform", `translate(${kidX},${childYTop})`);
        this.renderer.setAttribute(newKid, "d", `M0,${childYBottom - childYTop} v-${childYBottom - childYTop - 2 * this._node_roundAngle} a${2 * this._node_roundAngle},${2 * this._node_roundAngle} 0 0 1 ${2 * this._node_roundAngle},-${2 * this._node_roundAngle}`);
      } else if (roundedCorners && i == coordinates.length - 1) {
        childYTop--;
        kidX -= this._node_roundAngle;
        //this is the right corner
        //<path _ngcontent-xod-c308="" transform="translate(-18,158)" d="M0,0 a4,4 0 0 1 4,-4" class="node-link"></path>
        this.renderer.setAttribute(newKid, "transform", `translate(${kidX},${childYTop})`);
        this.renderer.setAttribute(newKid, "d", `M0,0 a${2 * this._node_roundAngle},${2 * this._node_roundAngle} 0 0 1 ${2 * this._node_roundAngle},${2 * this._node_roundAngle} v${childYBottom - childYTop - 2 * this._node_roundAngle}`);
      } else {
        this.renderer.setAttribute(newKid, "x1", kidX.toString());
        this.renderer.setAttribute(newKid, "y1", childYTop.toString());
        this.renderer.setAttribute(newKid, "x2", kidX.toString());
        this.renderer.setAttribute(newKid, "y2", childYBottom.toString());
      }
    }
  }

  private createNewNode(currentNodeInfo: TreeViewUINode) {

    var nodeGroup = this.renderer.createElement("g", "svg");
    var nodeCard = this.renderer.createElement("path", "svg");

    var fingerprint_icon = this.renderer.createElement("path", "svg");
    var expand_icon = this.renderer.createElement("path", "svg");

    var nameContent = this.renderer.createElement("foreignObject", "svg");
    var rulesContent = this.renderer.createElement("foreignObject", "svg");
    var nameText = this.renderer.createElement("div");
    var rulesText = this.renderer.createElement("div");
    var tooltip = this.renderer.createElement("title", "svg");
    var guid = this.renderer.createText(currentNodeInfo.treeNode.ElementId);

    var internalMargin = 20;
    var iconSize = 24;

    this.renderer.setAttribute(nodeGroup, "class", "node");

    this.renderer.setAttribute(nodeCard, "d", this._node_card_shape);

    this.renderer.setAttribute(fingerprint_icon, "d", this._finger_print_icon);

    this.renderer.setAttribute(nodeCard, "class", "node-card");
    this.renderer.setAttribute(fingerprint_icon, "class", "active-icon");
    this.renderer.setAttribute(fingerprint_icon, "transform", `translate(${internalMargin - this._node_roundAngle},${internalMargin})`);
    this.renderer.setAttribute(nameContent, "transform", `translate(${internalMargin - this._node_roundAngle + iconSize + (internalMargin - this._node_roundAngle) / 2},${internalMargin - 1})`);
    this.renderer.setAttribute(nameContent, "width", `${this._node_width - (internalMargin - this._node_roundAngle + iconSize + (internalMargin - this._node_roundAngle) / 2) - internalMargin / 2 + this._node_roundAngle}`);
    this.renderer.setAttribute(nameContent, "height", "28"); //css font height style external dependency, need clean up
    this.renderer.setAttribute(nameText, "class", "node-text");

    this.renderer.setAttribute(rulesContent, "transform", `translate(${internalMargin / 2},${internalMargin + 28})`);
    this.renderer.setAttribute(rulesContent, "width", `${this._node_width - internalMargin}`);
    this.renderer.setAttribute(rulesContent, "height", "16"); //css font height style external dependency, need clean up
    this.renderer.setAttribute(rulesText, "class", "node-text node-subtext");

    this.renderer.appendChild(nodeGroup, nodeCard);
    this.renderer.appendChild(nodeGroup, fingerprint_icon);
    this.renderer.appendChild(nodeGroup, nameContent);
    this.renderer.appendChild(nodeGroup, rulesContent);
    this.renderer.appendChild(nameContent, nameText);
    this.renderer.appendChild(nameText, this.renderer.createText(currentNodeInfo.treeNode.RealElement?.Name ?? "Null"));
    this.renderer.appendChild(rulesContent, rulesText);
    this.renderer.appendChild(rulesText, this.renderer.createText(currentNodeInfo.treeNode.RealElement?.RulesContent ?? "Null"));

    var iconMargin = 14;
    var iconVertical = internalMargin * 2 + 28 + 16;
    var iconSpace = (this._node_width + (this._node_roundAngle * 2) - (iconMargin * 2)) / 5;
    var firstStep = iconMargin;
    var i = 0;
    var icons: IconInfo[] = [
      {
        path: this._optional_icon,
        active: currentNodeInfo.treeNode.RealElement?.IsOptional ?? false
      },
      {
        path: this._repeat_icon,
        active: currentNodeInfo.treeNode.RealElement?.IsRepetition ?? false
      },
      {
        path: this._group_icon,
        active: currentNodeInfo.treeNode.RealElement?.IsGroup ?? false
      },
      {
        path: this._branch_icon,
        active: currentNodeInfo.treeNode.RealElement?.IsAlternation ?? false
      },
      {
        path: this._leaf_icon,
        active: currentNodeInfo.treeNode.RealElement?.IsLeaf ?? false
      }];
    for (var i = 0; i < icons.length; i++) {
      this.insertIcon(nodeGroup, firstStep + iconSpace * i, iconVertical, icons[i]);
    }

    if (!currentNodeInfo.treeNode.RealElement?.IsLeaf) {
      var expand_button = this.renderer.createElement("circle", "svg");
      var buttonRadius = 16;
      this.renderer.setAttribute(expand_button, "r", `${buttonRadius}px`);
      this.renderer.setAttribute(expand_button, "cx", `${this._node_width / 2}px`);
      this.renderer.setAttribute(expand_button, "cy", `${iconVertical + iconSpace + internalMargin}`);
      this.renderer.setAttribute(expand_button, "class", "node-card node-expand-button");

      var lastIconX = (this._node_width / 2) - (iconSize / 2);
      var lastIconY = iconVertical + iconSpace + internalMargin - iconSize / 2;
      this.renderer.setAttribute(expand_icon, "class", `active-icon`);
      this.renderer.setAttribute(expand_icon, "transform", `translate(${lastIconX},${lastIconY})`);

      this.expansionIcon = expand_icon;

      this.renderer.setAttribute(expand_icon, "d", this._expand_more_icon);
      this.renderer.appendChild(nodeGroup, expand_button);
      this.renderer.appendChild(nodeGroup, expand_icon);

      expand_button.addEventListener("click", () => {
        this.dispatchEvent(this._expandClick);
      });
    }

    this.renderer.appendChild(tooltip, guid);
    this.renderer.appendChild(fingerprint_icon, tooltip);

    this.domObject = nodeGroup;
  }

  private insertIcon(parent: any, xPos: number, yPos: number, iconInfo: IconInfo) {

    var iconGroup = this.renderer.createElement("g", "svg");
    var iconContainer = this.renderer.createElement("svg", "svg");
    this.renderer.setAttribute(iconContainer, "width", "32");
    this.renderer.setAttribute(iconContainer, "height", "32");
    this.renderer.setAttribute(iconContainer, "viewBox", "0 0 32 32");
    this.renderer.setAttribute(iconGroup, "transform", `translate(${xPos},${yPos})`);

    var icon = this.renderer.createElement("path", "svg");
    this.renderer.setAttribute(icon, "d", iconInfo.path);
    this.renderer.setAttribute(icon, "class", `${iconInfo.active ? "active-icon" : "passive-icon"}`);

    this.renderer.appendChild(parent, iconGroup);
    this.renderer.appendChild(iconGroup, iconContainer);
    this.renderer.appendChild(iconContainer, icon);
  }
}

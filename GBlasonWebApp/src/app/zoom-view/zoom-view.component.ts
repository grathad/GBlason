import { AfterViewInit, Component, Renderer2, ElementRef, Input, OnInit, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { Point } from '../tree-view/TreeViewUINode';

@Component({
  selector: 'app-zoom-view',
  templateUrl: './zoom-view.component.html',
  styleUrls: ['./zoom-view.component.scss']
})

export class ZoomViewComponent implements OnInit, AfterViewInit, OnChanges {

  constructor(private renderer: Renderer2) { }

  ngAfterViewInit(): void {
  }

  /**
   * View child of tree view component
   * canvas is the parent div container of the whole tree used to add the root(s) and to fill the display area
   * svg is the parent svg container of the whole tree, used to move the tree as one element
   */
  @ViewChild('canvas')
  canvasDom: ElementRef | null = null;
  @ViewChild('svg')
  svgDom: ElementRef | null = null;

  @Input("isActive")
  isActive: boolean = false;

  canvasCoordinateText: any;
  canvasObject: any;
  objectCoordinateText: any;
  objectCoordinateAbsText: any;
  cursCoordinateLocText: any;
  cursCoordinateAbsText: any;

  private _startDragPoint: Point = { x: 0, y: 0 };
  private _dragStarted: boolean = false;

  private _crossDebugZoomg: any = null;
  private _centerDebugPoint: Point = { x: 150, y: 150 };

  private _zoomScale: number = 100;
  private _zoomRoundingAccuracy = 4;

  private _canvasMatrix = [1, 0, 0, 1, 0, 0];
  private readonly _defaultMatrix = [1, 0, 0, 1, 0, 0];

  private uiCreated: boolean = false;

  //#region event handlers

  ngOnInit(): void {

  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes["isActive"] !== null) {
      if (changes["isActive"].currentValue === true)
        this.UiCreate();
    }
  }

  UiCreate(): void {
    if (this.uiCreated) {
      return;
    }
    this.uiCreated = true;
    //rendering the objects
    //background for the canvas (css style is enough)
    //the canvas coordinates on the top left (and init at 100,100)

    var canvasRect = this.renderer.createElement("rect", "svg");
    this.renderer.setAttribute(canvasRect, "width", "300");
    this.renderer.setAttribute(canvasRect, "height", "300");
    this.renderer.setAttribute(canvasRect, "class", "invisible");
    this.renderer.appendChild(this.canvasDom?.nativeElement, canvasRect);
    var canvasCoordinate = this.renderer.createElement("foreignObject", "svg");
    this.renderer.setAttribute(canvasCoordinate, "width", "200");
    this.renderer.setAttribute(canvasCoordinate, "height", "16");
    this.renderer.setAttribute(canvasCoordinate, "transform", "translate(20,20)");
    var canvasCoordinateTextDiv = this.renderer.createElement("div");
    this.renderer.appendChild(canvasCoordinate, canvasCoordinateTextDiv);
    this.renderer.setAttribute(canvasCoordinateTextDiv, "class", "node-text");
    this.canvasCoordinateText = this.renderer.createText("0,0");
    this.renderer.appendChild(canvasCoordinateTextDiv, this.canvasCoordinateText);

    //the main object in the canvas (init at 100,100 within the canvas itself)
    this.canvasObject = this.renderer.createElement("rect", "svg");
    this.renderer.setAttribute(this.canvasObject, "width", "100");
    this.renderer.setAttribute(this.canvasObject, "height", "100");
    this.renderer.setAttribute(this.canvasObject, "transform", "translate(100,100)");
    this.renderer.appendChild(this.canvasDom?.nativeElement, this.canvasObject);
    this.renderer.appendChild(this.canvasDom?.nativeElement, canvasCoordinate);
    //the main object coordinates in the canvas reference
    var objectCoordinate = this.renderer.createElement("foreignObject", "svg");
    this.renderer.setAttribute(objectCoordinate, "width", "100");
    this.renderer.setAttribute(objectCoordinate, "height", "32");
    this.renderer.setAttribute(objectCoordinate, "class", "node-text");
    this.renderer.setAttribute(objectCoordinate, "transform", "translate(100,100)");
    var objectCoordinateTextCanvas = this.renderer.createElement("div");
    this.objectCoordinateText = this.renderer.createText("loc: 100,100");
    var objectCoordinateTextCanvasAbs = this.renderer.createElement("div");
    this.objectCoordinateAbsText = this.renderer.createText("abs: 100,100");
    this.renderer.appendChild(objectCoordinate, objectCoordinateTextCanvas);
    this.renderer.appendChild(objectCoordinate, objectCoordinateTextCanvasAbs);
    this.renderer.appendChild(objectCoordinateTextCanvas, this.objectCoordinateText);
    this.renderer.appendChild(objectCoordinateTextCanvasAbs, this.objectCoordinateAbsText);
    this.renderer.appendChild(this.canvasDom?.nativeElement, objectCoordinate);

    //the debug cross
    this._crossDebugZoomg = this.renderer.createElement("g", "svg");
    var crossDebugZoomh = this.renderer.createElement("line", "svg");
    var crossDebugZoomv = this.renderer.createElement("line", "svg");
    //center of the debug cross should be
    this.renderer.setAttribute(crossDebugZoomh, "x1", (this._centerDebugPoint.x - 20).toString());
    this.renderer.setAttribute(crossDebugZoomh, "y1", (this._centerDebugPoint.y).toString());
    this.renderer.setAttribute(crossDebugZoomh, "x2", (this._centerDebugPoint.x + 20).toString());
    this.renderer.setAttribute(crossDebugZoomh, "y2", (this._centerDebugPoint.y).toString());
    this.renderer.setAttribute(crossDebugZoomv, "x1", (this._centerDebugPoint.x).toString());
    this.renderer.setAttribute(crossDebugZoomv, "y1", (this._centerDebugPoint.y - 20).toString());
    this.renderer.setAttribute(crossDebugZoomv, "x2", (this._centerDebugPoint.x).toString());
    this.renderer.setAttribute(crossDebugZoomv, "y2", (this._centerDebugPoint.y + 20).toString());
    this.renderer.setAttribute(crossDebugZoomv, "class", "cross-debug");
    this.renderer.setAttribute(crossDebugZoomh, "class", "cross-debug");
    this.renderer.appendChild(this.canvasDom?.nativeElement, this._crossDebugZoomg);
    this.renderer.appendChild(this._crossDebugZoomg, crossDebugZoomh);
    this.renderer.appendChild(this._crossDebugZoomg, crossDebugZoomv);

    //the cursor for test where the zoom coordinates are piloted
    var cursCoordinate = this.renderer.createElement("foreignObject", "svg");
    this.renderer.setAttribute(cursCoordinate, "width", "100");
    this.renderer.setAttribute(cursCoordinate, "height", "32");
    this.renderer.setAttribute(cursCoordinate, "class", "node-text");
    this.renderer.setAttribute(cursCoordinate, "transform", `translate(${this._centerDebugPoint.x},${this._centerDebugPoint.y})`);

    var cursCoordinateText = this.renderer.createElement("div");
    var cursCoordinateTextAbs = this.renderer.createElement("div");
    this.cursCoordinateLocText = this.renderer.createText("loc: 100,100");
    this.cursCoordinateAbsText = this.renderer.createText(`abs: ${this._centerDebugPoint.x},${this._centerDebugPoint.y}`);
    this.renderer.appendChild(cursCoordinate, cursCoordinateText);
    this.renderer.appendChild(cursCoordinate, cursCoordinateTextAbs);
    this.renderer.appendChild(cursCoordinateText, this.cursCoordinateLocText);
    this.renderer.appendChild(cursCoordinateTextAbs, this.cursCoordinateAbsText);
    this.renderer.appendChild(this.canvasDom?.nativeElement, cursCoordinate);

    this.UiLocationUpdate();
  }

  UiLocationUpdate() {
    //updating the text of all the location based on the new location of all the elements
    // canvasCoordinateText: any;
    // objectCoordinateText: any;
    // objectCoordinateAbsText: any;
    // cursCoordinateLocText: any;
    // cursCoordinateAbsText: any;
    var canvasRect = this.canvasDom?.nativeElement.getBoundingClientRect();
    var svgRect = this.svgDom?.nativeElement.getBoundingClientRect();

    var canvasRectLocX = this.roundNum(canvasRect.left - svgRect.left, this._zoomRoundingAccuracy);
    var canvasRectLocY = this.roundNum(canvasRect.top - svgRect.top, this._zoomRoundingAccuracy);

    this.renderer.setValue(this.canvasCoordinateText, `${canvasRectLocX},${canvasRectLocY}`);

    var canvasObjectRect = this.canvasObject.getBoundingClientRect();

    var objLocX = this.roundNum(canvasObjectRect.left - canvasRect.left, this._zoomRoundingAccuracy);
    var objLocY = this.roundNum(canvasObjectRect.top - canvasRect.top, this._zoomRoundingAccuracy);
    var objAbsX = this.roundNum(canvasObjectRect.left - svgRect.left, this._zoomRoundingAccuracy);
    var objAbsY = this.roundNum(canvasObjectRect.top - svgRect.top, this._zoomRoundingAccuracy);

    this.renderer.setValue(this.objectCoordinateText, `loc: ${objLocX},${objLocY}`);
    this.renderer.setValue(this.objectCoordinateAbsText, `abs: ${objAbsX},${objAbsY}`);

    var cursAbsX = this.roundNum(this._centerDebugPoint.x + canvasRectLocX, this._zoomRoundingAccuracy);
    var cursAbsY = this.roundNum(this._centerDebugPoint.y + canvasRectLocY, this._zoomRoundingAccuracy);
    this.renderer.setValue(this.cursCoordinateAbsText, `abs: ${cursAbsX},${cursAbsY}`);

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
    var delta = event.wheelDelta > 0 ? 10 : -10;

    //not optimum ideally should be calculed once, and then on resize, but no performance issues at that stage
    var svgLocation = this.svgDom?.nativeElement.getBoundingClientRect();
    var canvasLocation = this.canvasDom?.nativeElement.getBoundingClientRect();

    var canvasTopLeft: Point = { x: canvasLocation.top - svgLocation.top, y: canvasLocation.left - svgLocation.left };

    //trying at the center
    var centerPoint = { x: event.clientX - canvasLocation.left, y: event.clientY - canvasLocation.top };
    console.log(`zoom-view-component.onWheel => canvas top left: (${canvasTopLeft.x}, ${canvasTopLeft.y})`);

    this.zoom(delta, centerPoint);
  }
  //#endregion

  //#region matrix and math methods on the tree

  /**
   * Pans the SVG canvas to the direction given by the dx and dy
   * @param dx the distance to move on the x axis
   * @param dy the distance to move on the y axis
   */
  private pan(dx: number, dy: number) {
    this._canvasMatrix[4] += dx;
    this._canvasMatrix[5] += dy;

    var newMatrix = `matrix(${this._canvasMatrix.join(' ')})`;
    this.renderer.setAttribute(this.canvasDom?.nativeElement, "transform", newMatrix);
    this.UiLocationUpdate();
  }

  private roundNum(num: number, length: number) {
    var number = Math.round(num * Math.pow(10, length)) / Math.pow(10, length);
    return number;
  }

  /**
   * Zooms the SVG by the given scale as a step in percentage of zoom direction, like +10 (=+10% zoom in) or -25 (=25% zoom out)
   * @param scale the percentage by which to zoom in or out
   * @param zoomCenter the center point from which to perform the zooming update
   */
  private zoom(scale: number, zoomCenter: Point) {
    var totalPercentDelta = this._zoomScale + scale;
    if (totalPercentDelta <= 10 || totalPercentDelta > 200) {
      return;
    }

    //information available from our perspective
    //The percentage increase step (scale)
    //The total percentage increase from the origin (totalPercentDelta = this._zoomScale + scale)
    //The previous percentage increase before the current step (this._zoomScale)
    //The position of the cursor representing the zoom center within the reference of the canvas (zoomCenter)

    var log = `tree-view-component.zoom(scale: ${scale}, zoomCenter: (${zoomCenter.x}, ${zoomCenter.y})) for a total zoom of ${totalPercentDelta}`;

    //information to calculate
    //the zoom transformation we apply to the canvas will grow or shrink the canvas, and all the points in it on the 2D axis
    //we need to calculate the new position of the x and y point of the zoom center AFTER the zoom is applied
    //the distance on BOTH axis will be affected by the ratio of totalPercentDelta/ this._zoomScale
    var zoomChangeRatio = this.roundNum(totalPercentDelta / this._zoomScale, this._zoomRoundingAccuracy);
    log += `\n\t> the current ratio of transformation is: ${zoomChangeRatio}`;
    var newCenterX = this.roundNum(zoomChangeRatio * zoomCenter.x, this._zoomRoundingAccuracy);
    var newCenterY = this.roundNum(zoomChangeRatio * zoomCenter.y, this._zoomRoundingAccuracy);
    log += `\n\t> the new expected location of the point centered on the cursor will be : (${newCenterX}, ${newCenterY})`;
    //Now that we know the future location of the zoom center point, we just need to correct the transformation to move it by as many pixels as the delta of the new and old location
    var transformXCorrection = this.roundNum(newCenterX - zoomCenter.x, this._zoomRoundingAccuracy);
    var transformYCorrection = this.roundNum(newCenterY - zoomCenter.y, this._zoomRoundingAccuracy);
    log += `\n\t> corresponding to a translate correction of : (${-transformXCorrection}, ${-transformYCorrection})`;

    //canvas matrix has 6 values noted from a to f matching an array [0-6]
    //[0] = a = previous coordinate system X axis factor to apply to new X value
    //[1] = b = previous coordinate system x axis factor to apply to new y value (always 0 in 2D)
    //[2] = c = previous coordinate system y axis factor to apply to new x value (always 0 in 2D)
    //[3] = d = previous coordinate system y axis factor to apply to new y value
    //[4] = e = direct x translation (the correction to keep the zoom centered on the cursor)
    //[5] = f = direct y translation (the correction to keep the zoom centered on the cursor)

    //of course we also need to apply the new total zoom from the origin factor to the zoom matrix
    //example if we are now at 120% we multiply the original value by 120/100 --> 1.2 (so 1 --> 1.2)

    for (var i = 0; i < 4; i++) {
      this._canvasMatrix[i] = this._defaultMatrix[i] * (totalPercentDelta / 100);
    }

    this._canvasMatrix[4] -= transformXCorrection;
    this._canvasMatrix[5] -= transformYCorrection;

    this._zoomScale = totalPercentDelta;

    var newMatrix = `matrix(${this._canvasMatrix.join(' ')})`;
    log += `\n\t> final ${newMatrix}`;
    this.renderer.setAttribute(this.canvasDom?.nativeElement, "transform", newMatrix);
    console.log(log);
    this.UiLocationUpdate();
  }

  //#endregion
}

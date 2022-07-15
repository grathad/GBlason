import { Component, Input, OnInit } from '@angular/core';
import { TreeViewNode } from '../ebnf/ebnf.component';

@Component({
  selector: 'app-tree-view',
  templateUrl: './tree-view.component.html',
  styleUrls: ['./tree-view.component.scss']
})
export class TreeViewComponent implements OnInit {

  constructor() { }

  @Input()
  treeHead: TreeViewNode | null = null;

  ngOnInit(): void {
  }

}

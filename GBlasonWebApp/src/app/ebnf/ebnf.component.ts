import { DataSource } from '@angular/cdk/collections';
import { HttpClient, HttpErrorResponse, HttpEvent, HttpParamsOptions, HttpRequest, HttpResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit } from '@angular/core';

@Component({
  selector: 'app-ebnf',
  templateUrl: './ebnf.component.html',
  styleUrls: ['./ebnf.component.css']
})

export class EbnfComponent implements OnInit {

  constructor(private http: HttpClient) { }

  rawEbnf: string = '';
  rawEbnfSuccess: boolean = false;
  requestError: HttpErrorResponse = new HttpErrorResponse({});

  requestRawInProgress = false;
  requestTreeInProgress = false;

  treeEbnf: TreeViewNode | null = null;

  ngOnInit(): void {
    this.getRawEbnf();
    this.getTreeEbnf();
  }

  getTreeEbnf(id: string = ""): void {
    var httpRequest = new HttpRequest("GET", "api/ebnf/tree/" + id, { responseType: "text" });
    var request = this.http.request<string>(httpRequest);

    this.requestTreeInProgress = true;
    request.subscribe({
      next: (data: HttpEvent<string>) => {
        var result = data as HttpResponse<string>;
        if (result !== null && result.body !== null && result.body !== undefined) {
          this.treeEbnf = JSON.parse(result.body);
        }
      },
      error: (err) => { this.requestTreeInProgress = false; },
      complete: () => { this.requestTreeInProgress = false; }
    });
  }

  getRawEbnf(): void {

    var httpRequest = new HttpRequest("GET", "api/ebnf/raw", { responseType: "text" });

    var request = this.http.request<string>(httpRequest);
    this.requestRawInProgress = true;
    request.subscribe({
      next: (data: HttpEvent<string>) => {
        var result = data as HttpResponse<string>;
        if (result !== null && result.body !== null && result.body !== undefined) {
          this.rawEbnf = result.body;
        }
      },
      error: (err) => {
        this.rawEbnfSuccess = false;
        this.requestRawInProgress = false;
        this.requestError = err;
      },
      complete: () => {
        this.rawEbnfSuccess = true;
        this.requestRawInProgress = false;
      }
    });
  }
}

export class TreeViewNode {
  ElementId: string = "";
  RealElement: TreeElementBase | null = null;
  Children: TreeViewNode[] | null = null;
  HasChildren: boolean = false;
  ReferenceToElement: string | null = null;
}

export interface TreeElementBase {
  Name: string;
  IsOptional: boolean;
  IsRepetition: boolean;
  IsAlternation: boolean;
  IsGroup: boolean;
  IsLeaf: boolean;
  RulesContent: string | null;
}

import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-http-error',
  templateUrl: './http-error.component.html',
  styleUrls: ['./http-error.component.scss']
})
export class HttpErrorComponent implements OnInit {

  @Input()
  error: HttpErrorResponse = new HttpErrorResponse({});

  constructor() { }

  ngOnInit(): void {
  }

}

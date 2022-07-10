import { HttpClient, HttpErrorResponse, HttpEvent, HttpParamsOptions, HttpRequest, HttpResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

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

  ngOnInit(): void {
    this.getRawEbnf();
  }

  getRawEbnf(): void {
    // this.http.get<string>("ebnf")
    //   .subscribe((data: string) => this.rawEbnf = data);

    var httpRequest = new HttpRequest("GET", "api/ebnf/raw", { responseType: "text" });

    var request = this.http.request<string>(httpRequest);
    request.subscribe({
      next: (data: HttpEvent<string>) => {
        var result = data as HttpResponse<string>;
        if(result !== null && result.body !== null){
          this.rawEbnf = result.body;
        }
      },
      error: (err) => {
        this.rawEbnfSuccess = false;
        this.requestError = err;
      },
      complete: () => {
        this.rawEbnfSuccess = true
      }
    });
  }

  getEbnfTree(): void {

  }
}

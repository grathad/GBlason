import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-main-toolbar-menu',
  templateUrl: './main-toolbar-menu.component.html',
  styleUrls: ['./main-toolbar-menu.component.css']
})
export class MainToolbarMenuComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  navigate(target:string) {
    this.router.navigate(['/'+target]);
  }

  title = 'GBlason';
}

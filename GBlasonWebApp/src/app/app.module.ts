import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {MaterialModules} from '../material.modules';
import { MainToolbarMenuComponent } from './main-toolbar-menu/main-toolbar-menu.component';
import { AppRoutingModule } from './app-routing.module';
import { WelcomeComponent } from './welcome/welcome.component';
import { EbnfComponent } from './ebnf/ebnf.component';
import { BlasonParsingComponent } from './blason-parsing/blason-parsing.component';
import { CoatOfArmEditorComponent } from './coat-of-arm-editor/coat-of-arm-editor.component';


@NgModule({
  declarations: [
    AppComponent,
    MainToolbarMenuComponent,
    WelcomeComponent,
    EbnfComponent,
    BlasonParsingComponent,
    CoatOfArmEditorComponent,
  ],
  imports: [
    BrowserModule, HttpClientModule,
    BrowserAnimationsModule, MaterialModules, AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

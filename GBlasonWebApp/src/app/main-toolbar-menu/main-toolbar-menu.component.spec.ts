import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainToolbarMenuComponent } from './main-toolbar-menu.component';

describe('MainToolbarMenuComponent', () => {
  let component: MainToolbarMenuComponent;
  let fixture: ComponentFixture<MainToolbarMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MainToolbarMenuComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainToolbarMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

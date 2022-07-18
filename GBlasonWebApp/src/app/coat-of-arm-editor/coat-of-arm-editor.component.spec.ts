import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoatOfArmEditorComponent } from './coat-of-arm-editor.component';

describe('CoatOfArmEditorComponent', () => {
  let component: CoatOfArmEditorComponent;
  let fixture: ComponentFixture<CoatOfArmEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoatOfArmEditorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CoatOfArmEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

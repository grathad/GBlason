import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlasonParsingComponent } from './blason-parsing.component';

describe('BlasonParsingComponent', () => {
  let component: BlasonParsingComponent;
  let fixture: ComponentFixture<BlasonParsingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BlasonParsingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlasonParsingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EbnfComponent } from './ebnf.component';

describe('EbnfComponent', () => {
  let component: EbnfComponent;
  let fixture: ComponentFixture<EbnfComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EbnfComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EbnfComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

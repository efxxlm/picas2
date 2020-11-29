import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManejoAnticipoComponent } from './manejo-anticipo.component';

describe('ManejoAnticipoComponent', () => {
  let component: ManejoAnticipoComponent;
  let fixture: ComponentFixture<ManejoAnticipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManejoAnticipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManejoAnticipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleManejoAnticipoComponent } from './detalle-manejo-anticipo.component';

describe('DetalleManejoAnticipoComponent', () => {
  let component: DetalleManejoAnticipoComponent;
  let fixture: ComponentFixture<DetalleManejoAnticipoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleManejoAnticipoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleManejoAnticipoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

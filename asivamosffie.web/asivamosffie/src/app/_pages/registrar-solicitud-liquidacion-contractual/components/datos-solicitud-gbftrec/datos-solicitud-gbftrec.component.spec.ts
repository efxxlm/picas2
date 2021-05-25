import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosSolicitudGbftrecComponent } from './datos-solicitud-gbftrec.component';

describe('DatosSolicitudGbftrecComponent', () => {
  let component: DatosSolicitudGbftrecComponent;
  let fixture: ComponentFixture<DatosSolicitudGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatosSolicitudGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatosSolicitudGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

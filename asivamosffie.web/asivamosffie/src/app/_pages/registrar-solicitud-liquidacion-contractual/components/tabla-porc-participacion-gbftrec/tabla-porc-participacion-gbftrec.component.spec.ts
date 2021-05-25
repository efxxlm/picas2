import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaPorcParticipacionGbftrecComponent } from './tabla-porc-participacion-gbftrec.component';

describe('TablaPorcParticipacionGbftrecComponent', () => {
  let component: TablaPorcParticipacionGbftrecComponent;
  let fixture: ComponentFixture<TablaPorcParticipacionGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaPorcParticipacionGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaPorcParticipacionGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

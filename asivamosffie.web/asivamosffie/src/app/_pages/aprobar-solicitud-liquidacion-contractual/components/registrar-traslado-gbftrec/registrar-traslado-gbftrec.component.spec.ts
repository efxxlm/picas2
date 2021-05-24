import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarTrasladoGbftrecComponent } from './registrar-traslado-gbftrec.component';

describe('RegistrarTrasladoGbftrecComponent', () => {
  let component: RegistrarTrasladoGbftrecComponent;
  let fixture: ComponentFixture<RegistrarTrasladoGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarTrasladoGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarTrasladoGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

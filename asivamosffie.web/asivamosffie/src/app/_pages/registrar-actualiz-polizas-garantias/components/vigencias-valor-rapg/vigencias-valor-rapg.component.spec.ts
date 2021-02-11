import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VigenciasValorRapgComponent } from './vigencias-valor-rapg.component';

describe('VigenciasValorRapgComponent', () => {
  let component: VigenciasValorRapgComponent;
  let fixture: ComponentFixture<VigenciasValorRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VigenciasValorRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VigenciasValorRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

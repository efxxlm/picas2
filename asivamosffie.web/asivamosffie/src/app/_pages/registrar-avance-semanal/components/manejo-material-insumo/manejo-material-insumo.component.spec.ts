import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManejoMaterialInsumoComponent } from './manejo-material-insumo.component';

describe('ManejoMaterialInsumoComponent', () => {
  let component: ManejoMaterialInsumoComponent;
  let fixture: ComponentFixture<ManejoMaterialInsumoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManejoMaterialInsumoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManejoMaterialInsumoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

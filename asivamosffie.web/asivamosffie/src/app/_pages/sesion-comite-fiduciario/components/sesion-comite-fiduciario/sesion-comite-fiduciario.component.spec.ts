import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SesionComiteFiduciarioComponent } from './sesion-comite-fiduciario.component';

describe('SesionComiteFiduciarioComponent', () => {
  let component: SesionComiteFiduciarioComponent;
  let fixture: ComponentFixture<SesionComiteFiduciarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SesionComiteFiduciarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SesionComiteFiduciarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

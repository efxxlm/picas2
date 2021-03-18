import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarInformeFinalComponent } from './validar-informe-final.component';

describe('ValidarInformeFinalComponent', () => {
  let component: ValidarInformeFinalComponent;
  let fixture: ComponentFixture<ValidarInformeFinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarInformeFinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarInformeFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

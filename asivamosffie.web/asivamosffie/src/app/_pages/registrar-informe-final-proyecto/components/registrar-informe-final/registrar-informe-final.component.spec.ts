import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarInformeFinalComponent } from './registrar-informe-final.component';

describe('RegistrarInformeFinalComponent', () => {
  let component: RegistrarInformeFinalComponent;
  let fixture: ComponentFixture<RegistrarInformeFinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarInformeFinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarInformeFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

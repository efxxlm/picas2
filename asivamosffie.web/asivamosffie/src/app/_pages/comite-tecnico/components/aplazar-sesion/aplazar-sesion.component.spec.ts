import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AplazarSesionComponent } from './aplazar-sesion.component';

describe('AplazarSesionComponent', () => {
  let component: AplazarSesionComponent;
  let fixture: ComponentFixture<AplazarSesionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AplazarSesionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AplazarSesionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

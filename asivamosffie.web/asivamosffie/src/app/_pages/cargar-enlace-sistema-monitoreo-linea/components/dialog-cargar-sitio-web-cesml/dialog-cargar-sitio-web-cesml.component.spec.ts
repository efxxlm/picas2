import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCargarSitioWebCesmlComponent } from './dialog-cargar-sitio-web-cesml.component';

describe('DialogCargarSitioWebCesmlComponent', () => {
  let component: DialogCargarSitioWebCesmlComponent;
  let fixture: ComponentFixture<DialogCargarSitioWebCesmlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogCargarSitioWebCesmlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCargarSitioWebCesmlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

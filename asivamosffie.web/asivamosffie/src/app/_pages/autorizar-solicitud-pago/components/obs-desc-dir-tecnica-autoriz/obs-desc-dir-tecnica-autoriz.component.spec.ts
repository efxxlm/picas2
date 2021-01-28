import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsDescDirTecnicaAutorizComponent } from './obs-desc-dir-tecnica-autoriz.component';

describe('ObsDescDirTecnicaAutorizComponent', () => {
  let component: ObsDescDirTecnicaAutorizComponent;
  let fixture: ComponentFixture<ObsDescDirTecnicaAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsDescDirTecnicaAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsDescDirTecnicaAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

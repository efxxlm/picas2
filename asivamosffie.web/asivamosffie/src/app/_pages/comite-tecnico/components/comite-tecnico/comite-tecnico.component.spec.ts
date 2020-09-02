import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComiteTecnicoComponent } from './comite-tecnico.component';

describe('ComiteTecnicoComponent', () => {
  let component: ComiteTecnicoComponent;
  let fixture: ComponentFixture<ComiteTecnicoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComiteTecnicoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComiteTecnicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

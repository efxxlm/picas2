import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DevolverPorValidacionComponent } from './devolver-por-validacion.component';

describe('DevolverPorValidacionComponent', () => {
  let component: DevolverPorValidacionComponent;
  let fixture: ComponentFixture<DevolverPorValidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DevolverPorValidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DevolverPorValidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

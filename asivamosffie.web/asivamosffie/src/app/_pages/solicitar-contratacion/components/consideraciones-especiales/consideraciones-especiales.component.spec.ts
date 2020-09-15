import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsideracionesEspecialesComponent } from './consideraciones-especiales.component';

describe('ConsideracionesEspecialesComponent', () => {
  let component: ConsideracionesEspecialesComponent;
  let fixture: ComponentFixture<ConsideracionesEspecialesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConsideracionesEspecialesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConsideracionesEspecialesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

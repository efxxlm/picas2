import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarAvanceFisicoComponent } from './verificar-avance-fisico.component';

describe('VerificarAvanceFisicoComponent', () => {
  let component: VerificarAvanceFisicoComponent;
  let fixture: ComponentFixture<VerificarAvanceFisicoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarAvanceFisicoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarAvanceFisicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

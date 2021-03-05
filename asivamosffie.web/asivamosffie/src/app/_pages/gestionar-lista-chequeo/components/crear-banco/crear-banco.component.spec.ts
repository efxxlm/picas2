import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CrearBancoComponent } from './crear-banco.component';

describe('CrearBancoComponent', () => {
  let component: CrearBancoComponent;
  let fixture: ComponentFixture<CrearBancoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CrearBancoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CrearBancoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

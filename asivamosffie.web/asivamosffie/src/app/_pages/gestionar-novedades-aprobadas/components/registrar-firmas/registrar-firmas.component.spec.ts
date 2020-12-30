import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarFirmasComponent } from './registrar-firmas.component';

describe('RegistrarFirmasComponent', () => {
  let component: RegistrarFirmasComponent;
  let fixture: ComponentFixture<RegistrarFirmasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarFirmasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarFirmasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

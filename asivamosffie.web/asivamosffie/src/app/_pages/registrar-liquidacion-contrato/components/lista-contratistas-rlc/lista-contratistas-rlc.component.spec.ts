import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListaContratistasRlcComponent } from './lista-contratistas-rlc.component';

describe('ListaContratistasRlcComponent', () => {
  let component: ListaContratistasRlcComponent;
  let fixture: ComponentFixture<ListaContratistasRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListaContratistasRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListaContratistasRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

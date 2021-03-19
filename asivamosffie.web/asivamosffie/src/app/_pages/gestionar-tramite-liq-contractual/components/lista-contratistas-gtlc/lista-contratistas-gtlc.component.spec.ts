import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListaContratistasGtlcComponent } from './lista-contratistas-gtlc.component';

describe('ListaContratistasGtlcComponent', () => {
  let component: ListaContratistasGtlcComponent;
  let fixture: ComponentFixture<ListaContratistasGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListaContratistasGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListaContratistasGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

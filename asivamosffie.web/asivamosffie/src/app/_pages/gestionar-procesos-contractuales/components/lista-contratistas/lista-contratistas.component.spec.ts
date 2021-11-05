import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListaContratistasComponent } from './lista-contratistas.component';

describe('ListaContratistasComponent', () => {
  let component: ListaContratistasComponent;
  let fixture: ComponentFixture<ListaContratistasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListaContratistasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListaContratistasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListaChequeoRapgComponent } from './lista-chequeo-rapg.component';

describe('ListaChequeoRapgComponent', () => {
  let component: ListaChequeoRapgComponent;
  let fixture: ComponentFixture<ListaChequeoRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListaChequeoRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListaChequeoRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

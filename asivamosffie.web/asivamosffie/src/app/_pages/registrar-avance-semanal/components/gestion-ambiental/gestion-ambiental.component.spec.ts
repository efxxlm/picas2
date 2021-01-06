import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionAmbientalComponent } from './gestion-ambiental.component';

describe('GestionAmbientalComponent', () => {
  let component: GestionAmbientalComponent;
  let fixture: ComponentFixture<GestionAmbientalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionAmbientalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionAmbientalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

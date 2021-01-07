import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionSstComponent } from './gestion-sst.component';

describe('GestionSstComponent', () => {
  let component: GestionSstComponent;
  let fixture: ComponentFixture<GestionSstComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionSstComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionSstComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

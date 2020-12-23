import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionSSTComponent } from './gestion-sst.component';

describe('GestionSSTComponent', () => {
  let component: GestionSSTComponent;
  let fixture: ComponentFixture<GestionSSTComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionSSTComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionSSTComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

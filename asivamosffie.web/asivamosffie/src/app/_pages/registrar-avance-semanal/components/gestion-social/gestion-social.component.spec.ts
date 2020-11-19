import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionSocialComponent } from './gestion-social.component';

describe('GestionSocialComponent', () => {
  let component: GestionSocialComponent;
  let fixture: ComponentFixture<GestionSocialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionSocialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionSocialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

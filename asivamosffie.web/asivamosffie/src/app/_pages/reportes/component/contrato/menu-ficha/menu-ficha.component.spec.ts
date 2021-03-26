import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuFichaComponent } from './menu-ficha.component';

describe('MenuFichaComponent', () => {
  let component: MenuFichaComponent;
  let fixture: ComponentFixture<MenuFichaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MenuFichaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuFichaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

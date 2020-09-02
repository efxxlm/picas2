import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableFuentesYUsosComponent } from './table-fuentes-y-usos.component';

describe('TableFuentesYUsosComponent', () => {
  let component: TableFuentesYUsosComponent;
  let fixture: ComponentFixture<TableFuentesYUsosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableFuentesYUsosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableFuentesYUsosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableControlRecursosComponent } from './table-control-recursos.component';

describe('TableControlRecursosComponent', () => {
  let component: TableControlRecursosComponent;
  let fixture: ComponentFixture<TableControlRecursosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableControlRecursosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableControlRecursosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInfoFuenterecGogComponent } from './tabla-info-fuenterec-gog.component';

describe('TablaInfoFuenterecGogComponent', () => {
  let component: TablaInfoFuenterecGogComponent;
  let fixture: ComponentFixture<TablaInfoFuenterecGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInfoFuenterecGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInfoFuenterecGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

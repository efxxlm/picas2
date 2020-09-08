import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaProposicionesYVariosComponent } from './tabla-proposiciones-y-varios.component';

describe('TablaProposicionesYVariosComponent', () => {
  let component: TablaProposicionesYVariosComponent;
  let fixture: ComponentFixture<TablaProposicionesYVariosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaProposicionesYVariosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaProposicionesYVariosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

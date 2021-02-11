import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGeneralRapgComponent } from './tabla-general-rapg.component';

describe('TablaGeneralRapgComponent', () => {
  let component: TablaGeneralRapgComponent;
  let fixture: ComponentFixture<TablaGeneralRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGeneralRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGeneralRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

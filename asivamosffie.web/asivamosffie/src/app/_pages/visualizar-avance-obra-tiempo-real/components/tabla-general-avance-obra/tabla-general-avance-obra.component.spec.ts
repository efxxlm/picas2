import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGeneralAvanceObraComponent } from './tabla-general-avance-obra.component';

describe('TablaGeneralAvanceObraComponent', () => {
  let component: TablaGeneralAvanceObraComponent;
  let fixture: ComponentFixture<TablaGeneralAvanceObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGeneralAvanceObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGeneralAvanceObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

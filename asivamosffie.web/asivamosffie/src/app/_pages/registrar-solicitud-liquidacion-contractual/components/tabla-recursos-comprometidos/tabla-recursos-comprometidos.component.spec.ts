import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRecursosComprometidosComponent } from './tabla-recursos-comprometidos.component';

describe('TablaRecursosComprometidosComponent', () => {
  let component: TablaRecursosComprometidosComponent;
  let fixture: ComponentFixture<TablaRecursosComprometidosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRecursosComprometidosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRecursosComprometidosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

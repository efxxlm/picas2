import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DescDirTecnicaGogComponent } from './desc-dir-tecnica-gog.component';

describe('DescDirTecnicaGogComponent', () => {
  let component: DescDirTecnicaGogComponent;
  let fixture: ComponentFixture<DescDirTecnicaGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DescDirTecnicaGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DescDirTecnicaGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

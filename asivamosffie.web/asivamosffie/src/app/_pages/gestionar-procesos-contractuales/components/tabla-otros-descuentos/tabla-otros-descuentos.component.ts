import { Component, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-otros-descuentos',
  templateUrl: './tabla-otros-descuentos.component.html',
  styleUrls: ['./tabla-otros-descuentos.component.scss']
})
export class TablaOtrosDescuentosComponent implements OnInit {

  dataAportantes = new MatTableDataSource();
  @Input() displayedColumns: string[] = [];
  @Input() ELEMENT_DATA: any[] = [];

  constructor() { }

  ngOnInit(): void {
  }

}

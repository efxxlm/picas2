import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-recursos-aportantes',
  templateUrl: './tabla-recursos-aportantes.component.html',
  styleUrls: ['./tabla-recursos-aportantes.component.scss']
})
export class TablaRecursosAportantesComponent implements OnInit {

  dataSource                 = new MatTableDataSource();
  @Input() data   : any[]    = [];
  displayedColumns: string[] = [ 'nombre', 'valorAportante', 'uso', 'valorUso' ];

  constructor () { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource( this.data );
  };

}

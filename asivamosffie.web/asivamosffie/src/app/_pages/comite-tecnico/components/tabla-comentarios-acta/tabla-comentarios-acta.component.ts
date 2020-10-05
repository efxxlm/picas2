import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-comentarios-acta',
  templateUrl: './tabla-comentarios-acta.component.html',
  styleUrls: ['./tabla-comentarios-acta.component.scss']
})
export class tablaComentariosActaComponent implements OnInit, OnChanges {

  dataSource = new MatTableDataSource();
  @Input() data: any[];
  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, { static: true } ) sort: MatSort;
  displayedColumns: string[] = [ 'fecha', 'observacion', 'nombres' ];

  constructor() { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.data){
      this.dataSource = new MatTableDataSource( this.data );  
      console.log(changes, this.data)
    }
      
  }



  ngOnInit(): void {
    //this.dataSource = new MatTableDataSource( this.data );
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }

  textoLimpioMessage(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

};
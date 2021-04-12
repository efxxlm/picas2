import { AfterViewInit, Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-comentarios-acta',
  templateUrl: './tabla-comentarios-acta.component.html',
  styleUrls: ['./tabla-comentarios-acta.component.scss']
})
export class tablaComentariosActaComponent implements OnInit, OnChanges, AfterViewInit {

  dataSource = new MatTableDataSource();
  @Input() data: any[];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['fecha', 'observacion', 'nombres'];

  constructor() { }
  ngAfterViewInit(): void {
    
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.data) {
      
      this.dataSource = new MatTableDataSource(this.data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      //this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
      // this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      //   if (length === 0 || pageSize === 0) {
      //     return '0 de ' + length;
      //   }
      //   length = Math.max(length, 0);
      //   const startIndex = page * pageSize;
      //   // If the start index exceeds the list length, do not try and fix the end index to the end.
      //   const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
      //   return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
      // };

      console.log(changes, this.data)
    }

  }



  ngOnInit(): void {
    //this.dataSource = new MatTableDataSource( this.data );
    
  }

  textoLimpioMessage(texto: string) {
    if (texto) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

};
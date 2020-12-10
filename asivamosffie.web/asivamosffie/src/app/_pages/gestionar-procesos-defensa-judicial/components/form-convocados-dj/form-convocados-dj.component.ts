import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-form-convocados-dj',
  templateUrl: './form-convocados-dj.component.html',
  styleUrls: ['./form-convocados-dj.component.scss']
})
export class FormConvocadosDjComponent implements OnInit {
  displayedColumns: string[] = ['nomEntidadContratista', 'institucionEdu', 'codDane', 'sede', 'codSede', 'gestion'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable: any[] = [
    {
      nomEntidadContratista: 'Constructora Colpatria SAS',
      institucionEdu: 'Andres Bello',
      codDane: 'D435678',
      sede: 'Sede Principal',
      codSede: 'D435678',
      id: 1
    },
    {
      nomEntidadContratista: 'Constructora Colpatria SAS',
      institucionEdu: 'Andres Bello',
      codDane: 'D435678',
      sede: 'Sede 2',
      codSede: 'D435678',
      id: 2
    },
    {
      nomEntidadContratista: 'Constructora Colpatria SAS',
      institucionEdu: 'Andres Bello',
      codDane: 'D435678',
      sede: 'Sede 3',
      codSede: 'D435678',
      id: 3
    }
  ];
  formContratista: FormGroup;
  editorStyle = {
    height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  contratosArray = [
    { name: 'C223456789', value: '1' },
    { name: 'C223456999', value: '2' },
  ];

  constructor ( private fb: FormBuilder ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.formContratista.get( 'numeroContratos' ).valueChanges
      .subscribe( value => {
        this.perfiles.clear();
        for ( let i = 0; i < Number(value); i++ ) {
          this.perfiles.push( 
            this.fb.group(
              {
                contrato: [ null ],
                cvRequeridas: [ '' ],
                cvRecibidas: [ '' ],
                cvAprobadas: [ '' ],
                fechaAprobacionCv: [ null ],
                observaciones: [ null ],
                numeroRadicadoFfieAprobacionCv: this.fb.array([ [ '' ] ]),
                urlSoporte: [ '' ]
              }
            ) 
          )
        }
      } )
  };

	  get perfiles () {
		return this.formContratista.get( 'perfiles' ) as FormArray;
	  };

  get numeroRadicado () {
    let numero;
    Object.values( this.formContratista.controls ).forEach( control => {
      if ( control instanceof FormArray ) {
        Object.values( control.controls ).forEach( control => {
          numero = control.get( 'numeroRadicadoFfieAprobacionCv' ) as FormArray;
        } )
      }
    } )
    return numero;
  };
  seleccionAutocomplete(id:any){
    this.perfiles.value.contrato = id;
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
  }
  textoLimpio (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength (e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  crearFormulario () {
    this.formContratista = this.fb.group({
      numeroContratos: [ '' ],
      perfiles: this.fb.array([])
    });
  };

  eliminarPerfil ( numeroPerfil: number ) {
    this.perfiles.removeAt( numeroPerfil );
    this.formContratista.patchValue({
      numeroContratos: `${ this.perfiles.length }`
    });
  };

  agregarNumeroRadicado () {
    this.numeroRadicado.push( this.fb.control( '' ) )
  }

  eliminarNumeroRadicado ( numeroRadicado: number ) {
    this.numeroRadicado.removeAt( numeroRadicado );
  };

  guardar () {
    console.log( this.formContratista );
  }

}

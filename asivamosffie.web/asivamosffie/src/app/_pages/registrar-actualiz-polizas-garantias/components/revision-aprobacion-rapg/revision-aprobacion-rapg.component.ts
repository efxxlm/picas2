import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-revision-aprobacion-rapg',
  templateUrl: './revision-aprobacion-rapg.component.html',
  styleUrls: ['./revision-aprobacion-rapg.component.scss']
})
export class RevisionAprobacionRapgComponent implements OnInit {

    addressForm = this.fb.group({
        fechaRevision: [null, Validators.required],
        estadoRevision: [null, Validators.required],
        fechaAprob: [ null, Validators.required],
        responsableAprob: ['', Validators.required],
        observacionesGenerales: [ null ]
    });
    editorStyle = {
        height: '50px'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };
    estaEditando = false;
    //parametricas
    estadoArray = [];
    listaUsuarios: any[] = [];

    constructor(
        private fb: FormBuilder,
        private common: CommonService )
    {
        this.common.getUsuariosByPerfil( 10 ).subscribe(resp => {
          this.listaUsuarios = resp;
          console.log( resp )
        });

        this.common.listaEstadoRevision().subscribe(resp=>{
          this.estadoArray=resp;
        });
    }

    ngOnInit(): void {
    }
    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
      const alphanumeric = /[0-9]/;
      const inputChar = String.fromCharCode(event.charCode);
      return alphanumeric.test(inputChar) ? true : false;
    }
  
    maxLength( e: any, n: number ) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }
  
    onSubmit() {
      this.estaEditando = true;
    }

}

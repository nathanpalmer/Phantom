target foo:
	print "foo"

target xmlpeek_property:
	version_revision = xmlpeek('_revision.xml', '/info/entry/@revision', 'version.revision')
	print version_revision

target xmlpoke:
	xmlpoke(file: '_revision.xml', xpath: '/info/entry/@kind', value: 'TEST')
	